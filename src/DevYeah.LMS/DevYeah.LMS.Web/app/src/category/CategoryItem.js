import React from 'react';

export default function CategoryItem() {
  
  return (
    <div className="p-2 bg-success cat-item">
      <a className="cat-nav-link" href="#">
        <div className="cat-nav-color">
          <i className="far fa-file-code fa-2x"></i>
        </div>
        <span className="cat-font cat-nav-color">Frontend</span>
      </a>
    </div>
  );
}