import React, {Component} from 'react';
import ReactDom from 'react-dom';
import Board from './board';
import './style.scss';
import {User, UserThinking, UserWaiting} from './user';

const signalR = require("@microsoft/signalr");

const UserState = {
  TypeName: 1,
  ChooseBoard: 2,
  Playing: 3
}

const PlusSvg = <svg width="41" height="41" viewBox="0 0 41 41" xmlns="http://www.w3.org/2000/svg">
  <path
    d="M41 17.7045V23.2955C41 24.072 40.7282 24.732 40.1847 25.2756C39.6411 25.8191 38.9811 26.0909 38.2045 26.0909H26.0909V38.2045C26.0909 38.9811 25.8191 39.6411 25.2756 40.1847C24.732 40.7282 24.072 41 23.2955 41H17.7045C16.928 41 16.268 40.7282 15.7244 40.1847C15.1809 39.6411 14.9091 38.9811 14.9091 38.2045V26.0909H2.79545C2.01894 26.0909 1.3589 25.8191 0.815341 25.2756C0.27178 24.732 0 24.072 0 23.2955V17.7045C0 16.928 0.27178 16.268 0.815341 15.7244C1.3589 15.1809 2.01894 14.9091 2.79545 14.9091H14.9091V2.79545C14.9091 2.01894 15.1809 1.3589 15.7244 0.815341C16.268 0.27178 16.928 0 17.7045 0H23.2955C24.072 0 24.732 0.27178 25.2756 0.815341C25.8191 1.3589 26.0909 2.01894 26.0909 2.79545V14.9091H38.2045C38.9811 14.9091 39.6411 15.1809 40.1847 15.7244C40.7282 16.268 41 16.928 41 17.7045Z"/>
</svg>

class Main extends Component {
  constructor(props) {
    super(props);
    this.state = {
      userBoard: '',
      userName: '',
      boards: '',
      mode: UserState.TypeName,
      playing: false,
      connection: null,
      userTurn: false,
    }
    this.handleCreateBotBoard = this.handleCreateBotBoard.bind(this);
    this.handleNameChange = this.handleNameChange.bind(this);
    this.handleCreateBoard = this.handleCreateBoard.bind(this);
    this.onSubmitName = this.onSubmitName.bind(this);
  };

  componentDidMount() {
    let connection = new signalR.HubConnectionBuilder().withUrl("/gamethread").build();
    connection.start()
      .then(() => {
        console.log('Connection started!');
        connection.invoke("GetBoards");
      })
      .catch(err => console.log('Error while establishing connection :( '));

    connection.on("GetBoards", boards => {
      this.setState({boards});
      if (this.state.mode == UserState.Playing) {
        let userBoard = boards.find((element) => {
          return (element.id === this.state.userBoard.id)
        })
        let playing = false;
        if (userBoard.user1Name && userBoard.user2Name) {
          playing = true;
        }
        this.setState({userBoard: userBoard, playing: playing});
      }
    })

    connection.on("GetBoard", board => {
      console.log(board);
      this.setState({userBoard: board});
    })
    connection.on("JoinedBoard", (board, userTurn) => {
      this.setState({mode: UserState.Playing, userBoard: board, userTurn: userTurn})
    })
    this.setState({connection})
    this.setupBeforeUnloadListener();
  }

  setupInBoardHandle() {
    let connection = this.state.connection;
  }

  setupBeforeUnloadListener() {
    window.addEventListener("beforeunload", (ev) => {
      this.state.connection.invoke("Disconnect", this.state.userBoard.id);
    });
  };

  handleNameChange(e) {
    this.setState({
      userName: e.target.value.toUpperCase(),
    })
  }

  handleCreateBoard() {
    this.state.connection.invoke("CreateBoard", this.state.userName);
  }

  handleCreateBotBoard() {
    this.state.connection.invoke("CreateBotBoard", this.state.userName);
  }

  onSubmitName() {
    if (!this.state.userName) {
      alert("Type your name");
    }
    this.setState({mode: UserState.ChooseBoard});
  }

  handleJoinBoard(boardId) {
    if (this.state.userName) {
      this.state.connection.invoke("JoinBoard", boardId, this.state.userName);
    } else {
      alert("Type your name");
    }
  }

  render() {
    if (this.state.mode == UserState.TypeName) {
      return <div className='welcome-container'>
        <h1 className='welcome-text'>WELCOME TO CHESS</h1>
        <div className='type-to-start'>
          <span>Type</span>
          <input placeholder='your name' value={this.state.userName} onChange={this.handleNameChange}/>
          <span>to start</span>
        </div>
        <button onClick={this.onSubmitName} className='play-button'>Let's go</button>
      </div>
    } else if (this.state.mode == UserState.ChooseBoard) {
      return <div className='welcome-container'>
        <h1 className='welcome-text'>WELCOME TO CHESS</h1>
        <p className='chose-board'>
          Let's choose your board
        </p>
        <div className='list-board'>
          {this.state.boards &&
            this.state.boards.filter(value => (
                value.user1Name && !value.user2Name)
              || (!value.user1Name && value.user2Name)).map(value => {
              return (
                <div className="cell-board" onClick={() =>
                  this.handleJoinBoard(value.id)
                }>
                  <label>Play with {value.user1Name || value.user2Name}</label>
                </div>)
            })}

          <div className="create-bot-board" onClick={this.handleCreateBotBoard}>
            <label>Play with BOT</label>
          </div>
          <div className="create-board" onClick={this.handleCreateBoard}>
            <label>Play with your friend</label>
          </div>
        </div>
      </div>
    } else if (this.state.mode == UserState.Playing) {
      let color, opponentName;
      if (this.state.userBoard.user1Name == this.state.userName) {
        color = this.state.userBoard.user1Color === -1 ? 'black' : 'white';
        opponentName = this.state.userBoard.user2Name;
      } else if (this.state.userBoard.user2Name == this.state.userName) {
        color = this.state.userBoard.user2Color === -1 ? 'black' : 'white';
        opponentName = this.state.userBoard.user1Name;
      }
      return (
        <Board
          opponentName={opponentName}
          userName={this.state.userName}
          userTurn={this.state.userTurn}
          boardId={this.state.userBoard.id}
          userColor={color}
          playingMode={this.state.playing}
          connection={this.state.connection}/>
      )
    }
  }
}


ReactDom.render(<Main/>, document.getElementById('board'));