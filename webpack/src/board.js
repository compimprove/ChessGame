import React, {Component} from 'react';
import Square from './square';
import King from './piece/king';
import Queen from './piece/queen';
import Pawn from './piece/pawn';
import Bishop from './piece/bishop';
import Rook from './piece/rook';
import Knight from './piece/knight';
import SquareC from './squareC';
import MovingError from './errors/movingError';
import {User, UserThinking, UserWaiting} from './user';
import {debounce} from "lodash";

class Board extends Component {
  constructor(props) {
    super(props);
    this.boardRef = React.createRef();
    this.movingQueue = [];
    this.state = {
      board: null,
      flag: false,
      userTurn: this.props.userTurn,
      movingPiece: false
    }
    this.userColor = this.props.userColor
    this.handleSquareClick = debounce(this.handleSquareClick.bind(this), 300);
    this.loadBoard = this.loadBoard.bind(this);
    this.onClickContainer = this.onClickContainer.bind(this);
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

  moveInQueue() {
    if (!this._inMoveInQueue) {
      this._inMoveInQueue = true;
      this._moveAllInQueue();
    }
  }

  async _moveAllInQueue() {
    if (this.movingQueue.length > 0) {
      let {userName, coordChosen, coordClick, kingInDangerCoord} = this.movingQueue.shift()
      this.removeHighlight();
      await this.move(coordChosen, coordClick);
      if (kingInDangerCoord && kingInDangerCoord.length > 0) {
        kingInDangerCoord.forEach(coord => this.danger(coord))
      }
      this.setState({userTurn: userName !== this.props.userName});
      this._moveAllInQueue();
    } else {
      this._inMoveInQueue = false;
    }
  }

  componentDidMount() {
    this.props.connection.on("MovingPiece", (userName, coordChosen, coordClick, kingInDangerCoord) => {
      this.movingQueue.push({userName, coordChosen, coordClick, kingInDangerCoord});
      this.moveInQueue();
    })
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
    } else {
      this.setState({
        flag: true
      })
    }
  }

  hightLight(coord) {
    if (this.hightLightCoord.findIndex(hightLightCoord => hightLightCoord.col == coord.col && hightLightCoord.row == coord.row) != -1) return;
    let square = this.state.board[coord.row][coord.col];
    if (square.piece == null) {
      square.hightLight = "highlight";
    } else {
      square.hightLight = "canKilled";
    }
    this.hightLightCoord.push(coord);
  }

  danger(coord) {
    let square = this.state.board[coord.row][coord.col];
    square.hightLight = "inDangered";
    if (this.hightLightCoord.find(highLightCoord => highLightCoord.col == coord.col && highLightCoord.row == coord.row) == null) {
      this.hightLightCoord.push(coord);
    }
  }

  removeHighlight() {
    this.hightLightCoord.forEach(coord => {
      this.state.board[coord.row][coord.col].hightLight = null;
    });
    this.hightLightCoord = [];
    this.loadBoard();
  }

  async handleSquareClick(coord, piece) {
    console.log("square click");
    let havePiece = Boolean(piece);
    if (this.state.userTurn) {
      if (havePiece && piece.color == this.userColor) {
        await this.handleGenerateMove(coord);
      } else {
        await this.handleMove(coord);
      }
    } else {
      this.removeHighlight();
    }
    // } else await this.handleMove(coord);
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
      boardId: this.props.boardId,
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
        data.possibleMoves.forEach(coord => {
          this.hightLight(coord);
        })
        data.kingDangerMoves && data.kingDangerMoves.forEach(coord => {
          this.danger(coord)
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
    try {
      function findCoord(coordResult) {
        return (coordResult.row == coord.row && coordResult.col == coord.col);
      }

      if (this.hightLightCoord.findIndex(findCoord) >= 0) {
        this.movePiece(this.coordChosen, coord);
      } else {
        this.removeHighlight();
      }
    } catch (e) {
      if (e instanceof MovingError) {

      }
    }
  }

  movePiece(coordChosen, coordClick) {
    let data;
    let direction = '';
    if (this.userColor === "black")
      direction = "WhiteGodown";
    else if (this.userColor === "white") {
      direction = "WhiteGoup";
    } else
      throw "User Color code is wrong";

    data = {
      boardId: this.props.boardId,
      board: this.convertBoard(),
      coordChosen: coordChosen,
      coordClick: coordClick,
      direction: direction,
      userName: this.props.userName
    }

    this.props.connection.invoke("MovingPiece", data);
    //
    // let response = await fetch(this.apiMove, {
    //     method: 'POST',
    //     headers: {
    //         'Accept': 'application/json',
    //         'Content-Type': 'application/json'
    //     },
    //     body: JSON.stringify(data),
    // });
    // if (response.status === 200) {
    //     let result = await response.json();
    //     if (result) {
    //         this.setState({userTurn: false});
    //         this.removeHighlight();
    //         await this.move(coordChosen, coordClick);
    //     } else {
    //         throw MovingError();
    //     }
    // } else {
    //     throw MovingError();
    // }

  }

  async move(coordChosen, coordClick) {
    this.setState({
      movingPiece: {
        from: coordChosen,
        to: coordClick
      }
    });
    await new Promise(r => setTimeout(r, 500));
    let piece = this.state.board[coordChosen.row][coordChosen.col].removePiece();
    this.state.board[coordClick.row][coordClick.col].removePiece();
    this.state.board[coordClick.row][coordClick.col].addPiece(piece);
    this.setState({
      movingPiece: false
    });
  }

  onClickContainer(e) {
    if (e.target === this.boardRef.current) {
      this.removeHighlight();
    }
  }

  render() {
    let opponent;
    let user;
    if (this.props.opponentName) {
      if (this.state.userTurn) {
        opponent = <User name={this.props.opponentName}/>;
        user = <UserThinking name={this.props.userName}/>
      } else {
        opponent = <UserThinking name={this.props.opponentName}/>;
        user = <User name={this.props.userName}/>
      }
    } else {
      opponent = <UserWaiting/>;
      user = <User name={this.props.userName}/>
    }
    return (
      <div className='container' onClick={this.onClickContainer} ref={this.boardRef}>
        {opponent}
        <div className="board">
          {this.state.board.map((row, index) => (
            <>
              {row.map((squareC) => (
                <Square
                  movingPiece={this.state.movingPiece}
                  hightLight={squareC.hightLight}
                  color={squareC.color}
                  piece={squareC.piece}
                  coord={squareC.coord}
                  handleSquareClick={this.handleSquareClick}
                />
              ))}
            </>
          ))}
        </div>
        {user}
      </div>
    )
  }
}

function InitialBoard(userColor, otherColor) {
  let board = [];
  board[0] = [];
  let pieceRow1, pieceRow7;
  if (userColor == "white") {
    pieceRow1 = [
      new Rook(otherColor, board),
      new Knight(otherColor, board),
      new Bishop(otherColor, board),
      new Queen(otherColor, board),
      new King(otherColor, board),
      new Bishop(otherColor, board),
      new Knight(otherColor, board),
      new Rook(otherColor, board),
    ];
    pieceRow7 = [
      new Rook(userColor, board),
      new Knight(userColor, board),
      new Bishop(userColor, board),
      new Queen(userColor, board),
      new King(userColor, board),
      new Bishop(userColor, board),
      new Knight(userColor, board),
      new Rook(userColor, board),
    ]
  } else if (userColor == "black") {
    pieceRow1 = [
      new Rook(otherColor, board),
      new Knight(otherColor, board),
      new Bishop(otherColor, board),
      new King(otherColor, board),
      new Queen(otherColor, board),
      new Bishop(otherColor, board),
      new Knight(otherColor, board),
      new Rook(otherColor, board),
    ];
    pieceRow7 = [
      new Rook(userColor, board),
      new Knight(userColor, board),
      new Bishop(userColor, board),
      new King(userColor, board),
      new Queen(userColor, board),
      new Bishop(userColor, board),
      new Knight(userColor, board),
      new Rook(userColor, board),
    ]
  }

  for (let i = 0; i < 1; i++) {
    board[i] = [];
    for (let j = 0; j < 8; j++) {
      if ((i - j) % 2 == 0)
        board[i][j] = new SquareC("white", pieceRow1[j], {row: i, col: j});
      else
        board[i][j] = new SquareC("black", pieceRow1[j], {row: i, col: j});
    }
  }
  for (let i = 1; i < 2; i++) {
    board[i] = [];
    for (let j = 0; j < 8; j++) {
      let pawn = new Pawn(otherColor, board)
      if ((i - j) % 2 == 0)
        board[i][j] = new SquareC("white", pawn, {row: i, col: j});
      else
        board[i][j] = new SquareC("black", pawn, {row: i, col: j});
    }
  }
  for (let i = 2; i < 6; i++) {
    board[i] = [];
    for (let j = 0; j < 8; j++) {
      if ((i - j) % 2 == 0)
        board[i][j] = new SquareC("white", null, {row: i, col: j});
      else
        board[i][j] = new SquareC("black", null, {row: i, col: j});
    }
  }
  for (let i = 6; i < 7; i++) {
    board[i] = [];
    for (let j = 0; j < 8; j++) {
      let pawn = new Pawn(userColor, board, 'up')
      if ((i - j) % 2 == 0)
        board[i][j] = new SquareC("white", pawn, {row: i, col: j});
      else
        board[i][j] = new SquareC("black", pawn, {row: i, col: j});
    }
  }

  for (let i = 7; i < 8; i++) {
    board[i] = [];
    for (let j = 0; j < 8; j++) {
      if ((i - j) % 2 == 0)
        board[i][j] = new SquareC("white", pieceRow7[j], {row: i, col: j});
      else
        board[i][j] = new SquareC("black", pieceRow7[j], {row: i, col: j});
    }
  }
  return board;
}

export default Board