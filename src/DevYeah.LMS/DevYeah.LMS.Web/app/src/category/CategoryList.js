import React, { useState, useEffect } from 'react';
import CategoryItem from './CategoryItem';
import * as api from '../common/api';
import './category.css';

function isActiveCat(activeId, Id) {
    if (activeId == null) return false;
    return activeId === Id;
}

export default function CategoryList({activeCat}) {
  const [categories, setCategories] = useState([]);
  useEffect(() => {
    const fetchCategories = async () => {
      const response = await api.fetchAllCategories();
      setCategories(response.data);
    };

    fetchCategories();
  }, []);
  
  return (
    <div>
      <ul className="nav nav-fill">
        {categories.map((cat, index) => (
          <li 
            className="nav-item border"
            key={index}
          >
            <CategoryItem 
              data={cat}
              isSelected={isActiveCat(activeCat, cat.id)}
            />
          </li>
        ))}
      </ul>
    </div>
  );
}