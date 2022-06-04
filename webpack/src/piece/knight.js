import React, { Component } from 'react';

export default class Knight {
    constructor(color, board) {
        this.color = color;
        this.board = board;
        this.code = this.color + 'N';
    }
    imageUrl() {
        if (this.color == 'black')
            return "./images/wikipedia/bN.png";
        else if (this.color == 'white')
            return "./images/wikipedia/wN.png";
        else return "";
    }
}