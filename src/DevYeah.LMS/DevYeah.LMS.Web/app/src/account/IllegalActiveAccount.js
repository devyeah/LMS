import React from 'react';

export default function IllegalActiveAccount() {
  return (
    <div className="row align-items-center h-100">
      <div className="col-6 mx-auto">
        <div className="card form-center h-100 justify-content-center">
          <div>
            <h1 
              id="invalidActiveAccountHeader" 
              className="h4"
            >
              Sorry,
            </h1>
            <div className="alert alert-danger" role="alert">
              <span>We do not find any account that needs to be activated.</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}