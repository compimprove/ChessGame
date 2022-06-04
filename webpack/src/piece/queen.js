import React, { Component } from 'react';

export default class Queen {
    constructor(color, board) {
        this.color = color;
        this.board = board;
        this.code = this.color + 'Q';
    }
    imageUrl() {
        if (this.color == 'black')
            return "./images/wikipedia/bQ.png";
        else if (this.color == 'white')
            return "./images/wikipedia/wQ.png";
        else return ""
    }
}