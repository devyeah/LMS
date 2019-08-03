import React, { useState } from 'react';

export default function CategoryItem() {
  const [isHover, setIsHover] = useState(false);

  return (
    <div 
      className={"p-2 cat-item " + (isHover ? " bg-success" : "")}
      onMouseOver={() => setIsHover(true)}
      onMouseLeave={() => setIsHover(false)}
    >
      <a className="cat-nav-link" href="#">
        <div className={isHover ? "cat-nav-hover-color" : "cat-nav-color"}>
          <i className="far fa-file-code fa-2x"></i>
        </div>
        <span className={"cat-font " + (isHover ? "cat-nav-hover-color" : "cat-nav-color")}>Frontend</span>
      </a>
    </div>
  );
}