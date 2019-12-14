import React, { Component } from 'react';

export default class King {
    constructor(color, board) {
        this.color = color;
        this.board = board;
        this.code = this.color + 'K';
    }
    isKing() {
        return true;
    }
    image() {
        if (this.color == 'black')
            return (<img src="./images/wikipedia/bK.png" />)
        else if (this.color == 'white')
            return (<img src="./images/wikipedia/wK.png" />)
        else return (<span>??</span>)
    }
}