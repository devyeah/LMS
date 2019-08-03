import React from 'react';
import CategoryItem from './CategoryItem';
import './category.css';

export default function CategoryList() {
  //const [activeCat, setActiveCat] = useState();

  return (
    <div>
      <ul className="nav nav-fill">
        <li className="nav-item border">
          <CategoryItem />
        </li>
        <li className="nav-item border">
          <CategoryItem />
        </li>
        <li className="nav-item border">
          <CategoryItem />
        </li>
        <li className="nav-item border">
          <CategoryItem />
        </li>
        <li className="nav-item border">
          <CategoryItem />
        </li>
        <li className="nav-item border">
          <CategoryItem />
        </li>
        <li className="nav-item border">
          <CategoryItem />
        </li>
      </ul>
    </div>
  );
}