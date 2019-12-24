import React, { Component } from 'react';
import Square from './square';
import King from './piece/king';
import Queen from './piece/queen';
import Pawn from './piece/pawn';
import Bishop from './piece/bishop';
import Rook from './piece/rook';
import Knight from './piece/knight';
import SquareC from './squareC';

class Board extends Component {
    constructor(props) {
        super(props);
        this.state = {
            board: null,
            flag: false,
        }
        this.userColor = this.props.userColor
        this.handleSquareClick = this.handleSquareClick.bind(this);
        this.loadBoard = this.loadBoard.bind(this);
        this.userTurn = true;

        if (this.userColor === "white") {
            this.state.board = InitialBoard.call(this, "white", "black")
        } else if (this.userColor === "black") {
            this.state.board = InitialBoard.call(this, "black", "white")
        } else {
            throw 'UserColor code is wrong';
        }
        this.apiGenerateMove = 'api/Board/GeneratePossibleMove';
        this.apiMove = 'api/Board/Move';
        this.cordChosen = null;
        this.hightLightCoord = [];
        this.removeHighlight = this.removeHighlight.bind(this);
        this.hightLight = this.hightLight.bind(this);
    }

    convertBoard() {
        let board = this.state.board;
        let boardString = [];
        for (let row = 0; row < 8; row++) {
            boardString[row] = [];
            for (let col = 0; col < 8; col++) {
                if (board[row][col].piece == null) {  // No Piece
                    boardString[row][col] = null
                } else {    // Have piece
                    try {
                        boardString[row][col] = board[row][col].piece.code
                    } catch (e) {
                        throw "Piece didn't have code"
                    }
                }
            }
        }
        return boardString;
    }

    loadBoard() {
        if (this.state.flag) {
            this.setState({
                flag: false
            })
        }
        else {
            this.setState({
                flag: true
            })
        }
    }

    hightLight(coord) {
        let square = this.state.board[coord.row][coord.col];
        if (square.piece == null) {
            square.color = "lightblue";
        } else {
            square.color = "orange";
        }
        this.hightLightCoord.push(coord);
    }

    removeHighlight() {
        if (this.hightLightCoord == null) return;
        this.hightLightCoord.forEach(coord => {
            this.state.board[coord.row][coord.col].color =
                ((coord.row - coord.col) % 2 == 0) ? "white" : "black";
        });
        this.hightLightCoord = [];
        this.loadBoard();
    }

    async handleSquareClick(coord, piece) {
        let havePiece = Boolean(piece);
        if (havePiece && piece.color == this.userColor) {
            await this.handleGenerateMove(coord);
        } else {
            await this.handleMove(coord);
        }

    }

    async handleGenerateMove(coord) {
        this.removeHighlight();
        let data;
        let direction = '';
        if (this.userColor === "black")
            direction = "WhiteGodown";
        else if (this.userColor === "white") {
            direction = "WhiteGoup";
        } else
            throw "User Color code is wrong";

        data = {
            board: this.convertBoard(),
            coordClick: coord,
            direction: direction,
        }

        let response = await fetch(this.apiGenerateMove, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data),
        });
        if (response.status === 200) {
            await response.json().then(data => {
                data.forEach(coord => {
                    this.hightLight(coord);
                })
            });
        } else {

        }
        this.coordChosen = coord;
        this.loadBoard();
        //console.log(JSON.stringify(data));
    }

    async handleMove(coord) {
        if (this.props.playingMode == false) return;
        function findCoord(coordResult) {
            return (coordResult.row == coord.row && coordResult.col == coord.col);
        }
        if (this.hightLightCoord.findIndex(findCoord) >= 0) {
            await this.movePiece(this.coordChosen, coord);
        }
        this.removeHighlight();
    }

    async movePiece(coordChosen, coordClick) {
        let data;
        let direction = '';
        if (this.userColor === "black")
            direction = "WhiteGodown";
        else if (this.userColor === "white") {
            direction = "WhiteGoup";
        } else
            throw "User Color code is wrong";

        data = {
            board: this.convertBoard(),
            coordChosen: coordChosen,
            coordClick: coordClick,
            direction: direction,
        }

        let response = await fetch(this.apiMove, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data),
        });
        if (response.status === 200) {
            await response.json().then(result => {
                if (result) {
                    this.move(coordChosen, coordClick);
                    this.userTurn = false;
                }
            });
        } else {

        }

    }

    move(coordChosen, coordClick) {
        let piece = this.state.board[coordChosen.row][coordChosen.col].removePiece();
        this.state.board[coordClick.row][coordClick.col].removePiece();
        this.state.board[coordClick.row][coordClick.col].piece = piece;
        this.loadBoard();
    }

    render() {
        //console.log(this.state.board[0][0]);
        let array = [0, 1, 2, 3, 4, 5, 6, 7];
        return (
            <div className="row justify-content-center">
                <div>
                    <button className="square" style={{ marginBottom: '-6px' }} disabled>.</button><br />
                    {array.map(value => (
                        <>
                            <button className="square" style={{ marginBottom: '-1px' }} disabled>{value}</button>
                            <br />
                        </>
                    ))}
                </div>
                <div className="board">
                    <li className="row-square">
                        {array.map(value => (
                            <button className="square" disabled>{value}</button>
                        ))}
                    </li>

                    {this.state.board.map((row, index) => (
                        <li className="row-square" key={index}>
                            {row.map((squareC) => (
                                <Square
                                    color={squareC.color}
                                    piece={squareC.piece}
                                    coord={squareC.coord}
                                    handleSquareClick={this.handleSquareClick}
                                />
                            ))}
                        </li>
                    ))}
                </div>
                <div className="ml-3">
                    <button className="btn btn-outline-primary mb-3" onClick={this.removeHighlight}>Refresh</button>
                </div>
            </div>

        )
    }
}

function InitialBoard(userColor, otherColor) {
    let board = [];
    board[0] = [];
    let pieceRow1 = [
        new Rook(otherColor, board),
        new Knight(otherColor, board),
        new Bishop(otherColor, board),
        new Queen(otherColor, board),
        new King(otherColor, board),
        new Bishop(otherColor, board),
        new Knight(otherColor, board),
        new Rook(otherColor, board),
    ]
    for (let i = 0; i < 1; i++) {
        board[i] = [];
        for (let j = 0; j < 8; j++) {
            if ((i - j) % 2 == 0)
                board[i][j] = new SquareC("white", pieceRow1[j], { row: i, col: j });
            else
                board[i][j] = new SquareC("black", pieceRow1[j], { row: i, col: j });
        }
    }
    for (let i = 1; i < 2; i++) {
        board[i] = [];
        for (let j = 0; j < 8; j++) {
            let pawn = new Pawn(otherColor, board)
            if ((i - j) % 2 == 0)
                board[i][j] = new SquareC("white", pawn, { row: i, col: j });
            else
                board[i][j] = new SquareC("black", pawn, { row: i, col: j });
        }
    }
    for (let i = 2; i < 6; i++) {
        board[i] = [];
        for (let j = 0; j < 8; j++) {
            if ((i - j) % 2 == 0)
                board[i][j] = new SquareC("white", null, { row: i, col: j });
            else
                board[i][j] = new SquareC("black", null, { row: i, col: j });
        }
    }
    for (let i = 6; i < 7; i++) {
        board[i] = [];
        for (let j = 0; j < 8; j++) {
            let pawn = new Pawn(userColor, board, 'up')
            if ((i - j) % 2 == 0)
                board[i][j] = new SquareC("white", pawn, { row: i, col: j });
            else
                board[i][j] = new SquareC("black", pawn, { row: i, col: j });
        }
    }
    let pieceRow7 = [
        new Rook(userColor, board),
        new Knight(userColor, board),
        new Bishop(userColor, board),
        new Queen(userColor, board),
        new King(userColor, board),
        new Bishop(userColor, board),
        new Knight(userColor, board),
        new Rook(userColor, board),
    ]
    for (let i = 7; i < 8; i++) {
        board[i] = [];
        for (let j = 0; j < 8; j++) {
            if ((i - j) % 2 == 0)
                board[i][j] = new SquareC("white", pieceRow7[j], { row: i, col: j });
            else
                board[i][j] = new SquareC("black", pieceRow7[j], { row: i, col: j });
        }
    }
    return board;
}

export default Board