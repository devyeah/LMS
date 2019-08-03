import React from 'react';
import CategoryList from '../category/CategoryList';

export default function CourseListPage() {
  return (
    <div className="container">
      <div className="py-4">
        <CategoryList />
      </div>
      <div></div>
    </div>
  );
}