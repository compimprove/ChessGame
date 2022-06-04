import React, { Component } from 'react';

export default class SquareC {
    constructor(color, piece, coord) {
        this.color = color;
        this.piece = piece;
        this.coord = coord;
    }

    removePiece() {
        let piece = this.piece;
        this.piece = null;
        return piece;
    }

    addPiece(piece) {
        this.piece = piece;
    }
}