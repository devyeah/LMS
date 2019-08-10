import React, { useState, useEffect } from 'react';
import CourseItem from './CourseItem';
import * as api from '../common/api';
import './course.css';

export default function CourseList({activeCat}) {
  const [courses, setCourses] = useState([]);
  useEffect(() => {
    const courseItems = async () => {
      const response = await api.fetchCourses(activeCat);
      console.log(response.data);
      setCourses(response.data);
    };

    courseItems();
  }, [activeCat]);
  return (
    <div>
      {courses.map((course, index) => (
        <CourseItem 
          key={index}
          course={course}
        />
      ))}
    </div>
  );
}