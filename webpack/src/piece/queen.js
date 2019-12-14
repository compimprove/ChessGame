import React, { Component } from 'react';

export default class Queen {
    constructor(color, board) {
        this.color = color;
        this.board = board;
        this.code = this.color + 'Q';
    }
    image() {
        if (this.color == 'black')
            return (<img src="./images/wikipedia/bQ.png" />)
        else if (this.color == 'white')
            return (<img src="./images/wikipedia/wQ.png" />)
        else return (<span>??</span>)
    }
}