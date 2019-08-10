import React, { useState } from 'react';

export default function CourseItem({course}) {
  const [isSelected, toggleSelected] = useState(false);

  return (
    <a href="#" className="course-list-item">
      <div 
        className={"card mb-3 " + (isSelected ? "bg-light" : "bg-transparent")}
        onMouseOver={() => toggleSelected(true)}
        onMouseLeave={() => toggleSelected(false)}
      >
        <div className="card-body p-2 d-flex flex-row">
          <img 
            className="course-screenshot"
            src={course.screenCast} 
            alt="course screenshot" 
          />
          <div className="ml-3">
            <div>
              <h5 className="card-title mb-1">{course.name}</h5>
              <span 
                id="avgStdHours" 
                className="badge badge-success"
              >
                {course.avgLearningTime + ' '}hours
              </span>
              <span 
                id="courseLevel" 
                className="badge badge-warning ml-2"
              >
                Level: {course.level}
              </span>
            </div>
            <div className="mt-3">
              <p className="card-text">{course.overview}</p>
              <span>By {course.instructor.userName}</span>
            </div>
          </div>
        </div>
      </div>
    </a>
  );
}