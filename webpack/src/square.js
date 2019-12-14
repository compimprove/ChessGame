import React, { Component } from 'react';


class Square extends Component {
    constructor(props) {
        super(props);
        this.handleClick = this.handleClick.bind(this);
    }

    handleClick() {
        this.props.handleSquareClick(this.props.coord, this.props.piece);
    }

    render() {
        if (this.props.piece) {
            return (
                <button className={"square " + this.props.color} onClick={this.handleClick}>
                    {this.props.piece.image()}
                </button>
            )
        } else {
            return (
                <button className={"square " + this.props.color} onClick={this.handleClick}>
                </button>
            )
        }
    }
}

export default Square