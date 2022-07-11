import React from "react";

const LosePopup = ({handleClose, playAgain}) => {
  let popupBox = React.createRef();

  let onClickContainer = (e) => {
    if (e.target === popupBox.current) {
      handleClose()
    }
  }

  return (
    <div className="popup-box" onClick={onClickContainer} ref={popupBox}>
      <div className="lose">
        <h1>YOU LOSE -_-</h1>
        <button onClick={playAgain} style={{fontWeight: "bold", borderWidth: "2px", marginBottom: "10px"}}
                className='play-button-lose-1'>Try Again
        </button>
      </div>
    </div>
  );
};

export default LosePopup;