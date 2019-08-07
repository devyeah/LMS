import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import CategoryItem from './CategoryItem';
import * as api from '../common/api';
import './category.css';

function isActiveCat(activeCat, Id) {
    if (activeCat == null) return false;
    return activeCat.Id === Id;
}

export default function CategoryList() {
  const [categories, setCategories] = useState([]);
  const activeCat = useSelector(state => state => state.category.activeCategory);
  useEffect(() => {
    const fetchCategories = async () => {
      const cats = await api.fetchAllCategories();
      setCategories(cats);
    }

    fetchCategories();
  }, []);

  return (
    <div>
      <ul className="nav nav-fill">
        {categories.map(cat => (
          <li 
            key={cat.Id} 
            className="nav-item border"
          >
            <CategoryItem 
              data={cat}
              isSelected={isActiveCat(activeCat, cat.Id)}
            />
          </li>
        ))}
      </ul>
    </div>
  );
}