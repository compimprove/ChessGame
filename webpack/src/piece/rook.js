import React, { Component } from 'react';

export default class Rook {
    constructor(color, board) {
        this.color = color;
        this.board = board;
        this.code = this.color + 'R';
    }

    image() {
        if (this.color == 'black')
            return (<img src="./images/wikipedia/bR.png" />)
        else if (this.color == 'white')
            return (<img src="./images/wikipedia/wR.png" />)
        else return (<span>??</span>)
    }
}