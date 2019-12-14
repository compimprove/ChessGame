import React, { Component } from 'react';

export default class Bishop {
    constructor(color, board) {
        this.color = color;
        this.board = board;
        this.code = this.color + 'B';
    }
    image() {
        if (this.color == 'black')
            return (<img src="./images/wikipedia/bB.png" />)
        else if (this.color == 'white')
            return (<img src="./images/wikipedia/wB.png" />)
        else return (<span>??</span>)
    }
}