import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { switchCategory } from './redux/actions';

export default function CategoryItem({id, name, activeCat}) {
  const [isHover, setIsHover] = useState(false);
  const isChoosen = (activeCat == null) ? false : activeCat.Id === id;
  const isActive = isHover || isChoosen;
  const dispatch = useDispatch();

  return (
    <div 
      className={"p-2 cat-item " + (isActive ? " bg-success" : "")}
      onMouseOver={() => setIsHover(true)}
      onMouseLeave={() => setIsHover(false)}
      onClick={() => dispatch(switchCategory(id))}
    >
      <a className="cat-nav-link" href="#">
        <div className={isActive ? "cat-nav-hover-color" : "cat-nav-color"}>
          <i className="far fa-file-code fa-2x"></i>
        </div>
        <span className={"cat-font " + (isActive ? "cat-nav-hover-color" : "cat-nav-color")}>{name}</span>
      </a>
    </div>
  );
}