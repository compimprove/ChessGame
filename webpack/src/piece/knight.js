import React, { Component } from 'react';

export default class Knight {
    constructor(color, board) {
        this.color = color;
        this.board = board;
        this.code = this.color + 'N';
    }
    image() {
        if (this.color == 'black')
            return (<img src="./images/wikipedia/bN.png" />)
        else if (this.color == 'white')
            return (<img src="./images/wikipedia/wN.png" />)
        else return (<span>??</span>)
    }
}