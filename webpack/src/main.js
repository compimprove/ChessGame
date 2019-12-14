import React, { Component } from 'react';
import ReactDom from 'react-dom';
import Board from './board';
import './style.scss';
const signalR = require("@microsoft/signalr");


class Main extends Component {
    constructor(props) {
        super(props);
        this.state = {
            newBoardName: '',
            userName: '',
            boards: '',
            mode: 'outBoard',
            connection: null,
        }
        this.handleBoardNameChange = this.handleBoardNameChange.bind(this);
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleCreateBoard = this.handleCreateBoard.bind(this);
    };

    componentDidMount() {
        let connection = new signalR.HubConnectionBuilder().withUrl("/gamethread").build();
        connection.start()
            .then(() => {
                console.log('Connection started!');
                connection.invoke("GetBoards");
            })
            .catch(err => console.log('Error while establishing connection :('));
        connection.on("GetBoards", boards => {
            console.log(boards);
            this.setState({ boards });
        })
        this.setState({ connection })
    }
    
    handleNameChange(e) {
        this.setState({
            userName: e.target.value.toUpperCase(),
        })
    }
    handleBoardNameChange(e) {
        this.setState({
            newBoardName: e.target.value.toUpperCase(),
        })
    }

    handleCreateBoard() {
        if (this.state.newBoardName && this.state.userName) {
            this.state.connection.invoke("CreateBoard", this.state.newBoardName, this.state.userName);
        } else {
            alert("Type your name and board name");
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
                        </div>
                    </div>
                    <div className="row justify-content-center">
                        <div class="col-10">
                            <div class="form-group col-3">
                                <input type="text" className="form-control mb-2" placeholder="New Board" id="board-name" onChange={this.handleBoardNameChange} required />
                                <button className="btn btn-primary" onClick={this.handleCreateBoard}>+</button>
                            </div>

                        </div>
                    </div>
                </>
            )
        }
        else if (this.state.mode == 'inBoard') {
            return (
                <div>
                    <React.StrictMode>
                        <Board userColor="white" />
                    </React.StrictMode>
                </div>
            )
        }
    }
}


ReactDom.render(<Main />, document.getElementById('board'));