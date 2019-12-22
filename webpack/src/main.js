import React, { Component } from 'react';
import ReactDom from 'react-dom';
import Board from './board';
import './style.scss';
const signalR = require("@microsoft/signalr");


class Main extends Component {
    constructor(props) {
        super(props);
        this.state = {
            userBoardId: '',
            userName: '',
            userColor: 'white',
            boards: '',
            mode: 'outBoard',
            connection: null,
        }
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleColorChange = this.handleColorChange.bind(this);
        this.handleCreateBoard = this.handleCreateBoard.bind(this);
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
            console.log(boards);
            this.setState({ boards });
        })
        connection.on("JoinedBoard", (boardId) => {
            this.setState({ mode: "inBoard", userBoardId: boardId })
        })
        connection.onclose(() => {
            connection.invoke("Disconnect", this.state.userBoardId);
        })
        this.setState({ connection })
    }
    
    handleNameChange(e) {
        this.setState({
            userName: e.target.value.toUpperCase(),
        })
    }

    handleColorChange(e) {
        this.setState({ userColor: e.target.value })
    }

    handleCreateBoard() {
        if (this.state.userName) {
            this.state.connection.invoke("CreateBoard", this.state.userName);
        } else {
            alert("Type your name");
        }
    }
    handleJoinBoard(boardId) {
        if (this.state.userName) {
            this.state.connection.invoke("JoinBoard", boardId, this.state.userName);
        } else {
            alert("Type your name");
        }
    }
    render() {
        if (this.state.mode == 'outBoard') {
            return (
                <>
                    <div className="row justify-content-center mb-5">
                        <div class="form-group col-3">
                            <label className="mb-2" htmlFor="name">Type your name to start</label>
                            <input type="text" value={this.state.userName} className="form-control" id="name" onChange={this.handleNameChange} required />
                            <label className="mt-2 mr-2">Choose your color</label>
                            <select value={this.state.userColor} onChange={this.handleColorChange}>
                                <option value="white">White</option>
                                <option value="black">Black</option>
                            </select>
                        </div>
                    </div>
                    <div className="row justify-content-center">
                        <div className="col-2">
                            <button className="btn btn-primary btn-lg" onClick={this.handleCreateBoard}>New Board</button> Or
                        </div>
                        {this.state.boards &&
                            this.state.boards.map(value => (
                                <div className="col-2">
                                    <div className="cell-board">
                                        <button className="btn btn-danger" onClick={this.handleJoinBoard.bind(this, value.id)}>Join</button>
                                    </div>
                                    <label>{value.user1Name}</label>
                                </div>
                            ))}
                    </div>
                </>
            )
        }
        else if (this.state.mode == 'inBoard') {
            return (
                <div>
                    <Board userColor={this.state.userColor} />
                </div>
            )
        }
    }
}


ReactDom.render(<Main />, document.getElementById('board'));