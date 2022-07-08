import React, { Component } from 'react';


class Square extends Component {
    constructor(props) {
        super(props);
        this.buttonRef = React.createRef();
        this.handleClick = this.handleClick.bind(this);
    }

    handleClick() {
        this.props.handleSquareClick(this.props.coord, this.props.piece);
    }

    shouldMoving() {
        return this.props.movingPiece 
        && this.props.coord
        && this.props.coord.col == this.props.movingPiece.from.col 
        && this.props.coord.row == this.props.movingPiece.from.row;
    }

    calculateMoving() {
        const { from, to } = this.props.movingPiece;
        let x = this.buttonRef.current.clientWidth * (to.col - from.col);
        let y = this.buttonRef.current.clientHeight * (to.row - from.row);
        return { transform: `translate(${x}px, ${y}px)` };
    }

    render() {
        let highlightClass = this.props.hightLight || "";
        let style = {}
        if (this.shouldMoving()) {
            style = this.calculateMoving();
        }
        return (
            <button ref={this.buttonRef} className={"square " + `${this.props.color} ${highlightClass}`} onClick={this.handleClick}>
                {this.props.piece && <img style={style} src={this.props.piece.imageUrl()} />}
            </button>
        )
    }
}

export default Square