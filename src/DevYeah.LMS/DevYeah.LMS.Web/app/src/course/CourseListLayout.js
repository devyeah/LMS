import React from 'react';
import { useSelector } from 'react-redux';
import CategoryList from '../category/CategoryList';
import CourseList from './CourseList';

export default function CourseListLayout() {
  const activeCatId = useSelector(state => state.category.activeCategory);
  return (
    <div className="container">
      <div id="catNav" className="py-4">
        <CategoryList 
          activeCat={activeCatId} 
        />
      </div>
      <div id="courseList">
        <CourseList 
          activeCat={activeCatId} 
        />
      </div>
    </div>
  );
}