import React from 'react';

const BoardIcon = <svg viewBox="0 0 51 51" xmlns="http://www.w3.org/2000/svg">
    <path d="M0.083252 0.083313H50.9166V50.9166H0.083252V0.083313ZM5.16658 5.16665V15.3333H15.3333V25.5H5.16658V35.6666H15.3333V45.8333H25.4999V35.6666H35.6666V45.8333H45.8332V35.6666H35.6666V25.5H45.8332V15.3333H35.6666V5.16665H25.4999V15.3333H15.3333V5.16665H5.16658ZM25.4999 25.5H15.3333V35.6666H25.4999V25.5ZM25.4999 15.3333V25.5H35.6666V15.3333H25.4999Z" />
</svg>


export function UserThinking({ name }) {
    return <div className='user-normal'>
        <img src='./svg/time.svg' />
        <span>{name}</span>
    </div>
}

export function UserWaiting() {
    return <div className='user-normal'>
        <img src='./svg/loading.svg' />
        <span style={{ marginTop: "4px" }}>Waiting...</span>
    </div>
}

export function User({ name }) {
    return <div className='user-normal'>
        {BoardIcon}
        <span>{name}</span>
    </div>
}