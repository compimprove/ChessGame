import React, { Component } from 'react';

export default class Rook {
    constructor(color, board) {
        this.color = color;
        this.board = board;
        this.code = this.color + 'R';
    }

    imageUrl() {
        if (this.color == 'black')
            return "./images/wikipedia/bR.png";
        else if (this.color == 'white')
            return "./images/wikipedia/wR.png";
        else return ""
    }
}