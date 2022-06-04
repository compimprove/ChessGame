import React, { Component } from 'react';

export default class Bishop {
    constructor(color, board) {
        this.color = color;
        this.board = board;
        this.code = this.color + 'B';
    }
    imageUrl() {
        if (this.color == 'black')
            return "./images/wikipedia/bB.png"
        else if (this.color == 'white')
            return "./images/wikipedia/wB.png"
        else return ""
    }
}