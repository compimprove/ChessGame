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
    imageUrl() {
        if (this.color == 'black')
            return "./images/wikipedia/bK.png"
        else if (this.color == 'white')
            return "./images/wikipedia/wK.png"
        else return ""
    }
}