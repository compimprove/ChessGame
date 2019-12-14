import React, { Component } from 'react';

export default class Pawn {
    constructor(color, board, direction) {
        this.color = color;
        this.board = board;
        this.code = this.color + 'P';
        this.direction = direction;
    }
    image() {
        if (this.color == 'black')
            return (<img src="./images/wikipedia/bP.png" />)
        else if (this.color == 'white')
            return (<img src="./images/wikipedia/wP.png" />)
        else return (<span>??</span>)
    }
}