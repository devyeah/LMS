import React from 'react';

export default function FetchDataError() {
  return (
    <div className="card">
      <div className="card-body">
        <h5 id="fetchDataError" className="card-title">Sorry, something goes wrong.</h5>
        <p className="card-text">Please try again later.</p>
      </div>
    </div>
  );
}