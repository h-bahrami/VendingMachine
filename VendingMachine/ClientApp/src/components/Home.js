import React, { Component } from 'react';

var liStyle = {
    padding: '5px', listStyle: 'none'
}

export class Home extends Component {
    static displayName = Home.name;

    constructor() {
        super();
        this.state = {
            products: [],
            coins: [],
            selectedCoins: [],
            selectedProduct: null,
            result: null,
        };
        this.generateCoins = this.generateCoinButtons.bind(this);
        this.getSelectedCoins = this.showSelectedCoins.bind(this);
    }

    componentDidMount() {
        this.loadData();
    }

    render() {

        let productItems = [];
        this.state.products.forEach(value => {
            productItems.push(
                <li style={liStyle}>
                    <button onClick={() => this.setState({ selectedProduct: value, result: null})}
                        style={{ width: '150px' }}>
                        {value.name}
                    </button>
                </li>
            )
        });

        return (

            <div style={{ width: '900px' }}>

                <div style={{ float: 'left', width: '200px' }}>
                    <ul>{productItems}</ul>
                </div>
                <div style={{ float: 'left', width: '200px' }}>
                    {
                        this.state.selectedProduct ?
                            <ul>
                                <li style={liStyle} ><b>{this.state.selectedProduct.name}</b></li>
                                <li style={liStyle} >Price: {this.state.selectedProduct.price} &euro;</li>
                                <li style={liStyle} >Available: {this.state.selectedProduct.portions}</li>
                            </ul> :
                            null
                    }
                </div>
                <div style={{ float: 'left', width: '200px' }}>
                    {
                        this.state.selectedProduct ?
                            <ul>
                                {this.generateCoinButtons()}
                            </ul> :
                            null
                    }

                </div>

                <div style={{ float: 'left', width: '200px' }}>
                    {this.state.selectedProduct ? this.showSelectedCoins() : null}                 
                </div>
                <br style={{ clear: 'left' }} />
                <div style={{ float: 'left', width: '800px', padding: '30px 150px 30px 150px' }}>
                    {this.state.result ?
                        <b>{this.state.result.message}</b>
                        : null}

                    {this.showReturnedCoins()}
                </div>

            </div>
        );
    }

    showReturnedCoins() {
        let items = [];
        if (this.state.result && this.state.result.coins.length !== 0) {
            this.state.result.coins.forEach(x => {
                items.push(<li style={liStyle}>
                    {x.count} of {x.value < 1.0 ? `${x.value * 100} Cent(s) Coin` : `${x.value * 100} Euro(s) Coin`}
                </li>)
            });
        }
        return items;
    }

    amount() {
        let amount = 0
        this.state.selectedCoins.forEach(coin => {
            amount += coin.value * coin.count;
        });
        return amount;
    }

    async pay(coins) {

        let order = JSON.stringify({
            productId: this.state.selectedProduct.id,
            coins
        });
        console.log(order)
        let response = await fetch('api/Machine/Purchase', {
            method: 'post',
            body: order,
            headers: {
                'Content-Type': 'application/json',
            }
        });
        let message = await response.json();
        console.log(message);
        if (message.message === "Thank you") {
            this.setState({ result: message });
            this.loadData();
        } else {
            this.setState({ result: message });
        }
    }

    showSelectedCoins() {
        let items = [];
        this.state.selectedCoins.forEach(coin => {
            items.push(<li style={liStyle}>
                {coin.value < 1.0 ? `${coin.count} - ${coin.value * 100} Cent Coin` : `${coin.count} - ${coin.value} Euro Coin`}
            </li>)
        });
        let amount = this.amount();
        items.push(<li style={liStyle}><b>Total: {amount}</b></li>)
        return items;
    }

    generateCoinButtons() {
        let items = [];
        this.state.coins.forEach(coin => {
            items.push(<li style={liStyle}>
                <button onClick={() => this.addCoin(coin.value)} style={{ width: '150px' }}>
                    {coin.value < 1.0 ? `${coin.value * 100} Cent Coin` : `${coin.value} Euro Coin`}
                </button>
            </li>)
        });
        return items;
    }

    addCoin(value) {
        let selectedCoins = this.state.selectedCoins.slice(0);
        let item = selectedCoins.find(x => x.value === value);
        if (item) {
            item.count += 1;
        } else {
            selectedCoins = selectedCoins.concat({
                value,
                count: 1
            });
        }
        let amount = 0.0;
        selectedCoins.forEach(x => {
            amount += x.value * x.count;
        });

        this.setState({
            selectedCoins
        });

        if (amount >= this.state.selectedProduct.price) {
            this.pay(selectedCoins);
        }
    }


    loadData() {

        Promise.all([fetch('api/Machine/Products'), fetch('api/Machine/AcceptedCoins')])
            .then(async (values) => {
                let products = await values[0].json();
                let coins = await values[1].json();

                this.setState({ products, coins, selectedProduct: null, selectedCoins: [] });
            }).catch(reason => console.log(reason));
    }   
}
