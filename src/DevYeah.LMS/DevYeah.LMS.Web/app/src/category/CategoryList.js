import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import CategoryItem from './CategoryItem';
import * as api from '../common/api';
import './category.css';

function isActiveCat(activeCatId, Id) {
    if (activeCatId == null) return false;
    return activeCatId === Id;
}

export default function CategoryList() {
  const [categories, setCategories] = useState([]);
  const activeCatId = useSelector(state => state.category.activeCategory);
  useEffect(() => {
    const fetchCategories = async () => {
      const response = await api.fetchAllCategories();
      setCategories(response.data);
    }

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
              isSelected={isActiveCat(activeCatId, cat.id)}
            />
          </li>
        ))}
      </ul>
    </div>
  );
}