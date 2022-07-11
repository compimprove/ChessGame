import React from "react";

const WinPopup = ({handleClose, playAgain}) => {
  let popupBox = React.createRef();

  let onClickContainer = (e) => {
    if (e.target === popupBox.current) {
      handleClose()
    }
  }

  return (
    <div className="popup-box" onClick={onClickContainer} ref={popupBox}>
      <div className="win">
        <h1>YOU WIN ^-^</h1>
        <button onClick={playAgain} style={{fontWeight: "bold", borderWidth: "2px", marginBottom: "10px"}}
                className='play-button-win'>Play Again
        </button>
      </div>
    </div>
  );
};

export default WinPopup;