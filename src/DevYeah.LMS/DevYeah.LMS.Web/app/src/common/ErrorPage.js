import React from 'react';
import { Link } from 'react-router-dom';
import './common.css';

export default function ErrorPage() {
  return (
    <div id="notfound">
      <div className="notfound">
        <div className="notfound-404">
          <h1 id="errorTitle">Oops!</h1>
        </div>
        <h2 id="subErrorTitle">404 - Page not found</h2>
        <p id="errorContent">
          The page you are looking for might have been removed had its name changed or is temporarilly unavailable.
        </p>
        <Link 
          id="homePageLink"
          to="/"
        >
          Go To Homepage
        </Link>
      </div>
    </div>
  );
}