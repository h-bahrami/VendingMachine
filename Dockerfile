FROM node:10-alpine AS build-node
WORKDIR /src/VendingMachine/ClientApp
COPY VendingMachine/ClientApp/package.json .
COPY VendingMachine/ClientApp/package-lock.json .
RUN npm install
COPY VendingMachine/ClientApp/ .
RUN npm run build

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["VendingMachine/VendingMachine.csproj", "VendingMachine/"]
COPY ["VendingMachine.Services/VendingMachine.Services.csproj", "VendingMachine.Services/"]
COPY ["VendingMachine.Data/VendingMachine.Data.csproj", "VendingMachine.Data/"]
RUN dotnet restore "VendingMachine/VendingMachine.csproj"
COPY . .
WORKDIR "/src/VendingMachine"
RUN dotnet build "VendingMachine.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "VendingMachine.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VendingMachine.dll"]