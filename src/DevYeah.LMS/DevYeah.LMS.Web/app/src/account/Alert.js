import React from 'react';

export default function Alert({ message, type }) {
  const style = type === 'error' ? "alert alert-danger" : "alert alert-success";

  return (
    <div className={style} role="alert">
      {message}
      <button type="button" className="close" data-dismiss="alert" aria-label="Close">
        <span aria-hidden="true">&times;</span>
      </button>
    </div>
  );
}
