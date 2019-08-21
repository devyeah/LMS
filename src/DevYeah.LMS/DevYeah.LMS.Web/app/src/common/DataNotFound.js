import React from 'react';

export default function DataNotFound() {
  return (
    <div className="card">
      <div className="card-body">
        <h5 id="dataNotFoundNotice" className="card-title">Sorry, we couldn't find any results.</h5>
        <p className="card-text">Try adjusting your search.</p>
      </div>
    </div>
  );
}