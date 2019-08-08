import React, { useState, useEffect } from 'react';
import * as api from '../common/api';

export default function CourseList({activeCat}) {
  const [courses, setCourses] = useState([]);
  useEffect(() => {
    const courseItems = async () => {
      const response = await api.fetchCourses(activeCat);
      setCourses(response.data);
    };

    courseItems();
  }, [activeCat]);
  return (
    <div>
      {courses.map((course, index) => (
        <div key={index}>
          <span>{course.name}</span>
          <img src={course.screenCast} alt="img" />
          <span>{course.overview}</span>
        </div>
      ))}
    </div>
  );
}