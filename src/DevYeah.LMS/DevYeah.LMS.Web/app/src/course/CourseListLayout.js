import React from 'react';
import CategoryList from '../category/CategoryList';

export default function CourseListLayout() {
  return (
    <div className="container">
      <div id="catNav" className="py-4">
        <CategoryList />
      </div>
      <div id="courseList">

      </div>
      <div id="pagination">

      </div>
    </div>
  );
}