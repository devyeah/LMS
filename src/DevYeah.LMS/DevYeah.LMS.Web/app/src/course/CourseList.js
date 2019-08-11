import React, { useState, useEffect } from 'react';
import CourseItem from './CourseItem';
import * as api from '../common/api';
import './course.css';

export default function CourseList({activeCat}) {
  const [courseList, setCourseList] = useState([]);
  const [page, setPage] = useState(process.env.REACT_APP_FIRST_PAGE);
  const [pageSize, setPageSize] = useState(process.env.REACT_APP_DFAULT_PAGESIZE);
  const [pageNumbers, setPageNumbers] = useState([]);

  const fillList = async () => {
    const response = await api.fetchCourses(page, pageSize, activeCat);
    setCourseList(response.data.results);
    if (pageNumbers.length !== response.data.pageCount) {
      const totalPage = response.data.pageCount;
      const pageArr = [];
      for (let index = 1; index <= totalPage; index++) {
        pageArr[index - 1] = index; 
      }
      setPageNumbers(pageArr);
    }
  };

  useEffect(() => {
    fillList();
  }, [page, pageSize]);

  useEffect(() => {
    setPage(process.env.REACT_APP_FIRST_PAGE);
    setPageSize(process.env.REACT_APP_DFAULT_PAGESIZE);
    fillList();
  }, [activeCat]);

  return (
    <div>
      <div>
        {courseList.map((course, index) => (
          <CourseItem 
            key={index}
            course={course}
          />
        ))}
      </div>
      <nav id="pagination" aria-label="Page navigation">
        <ul className="pagination pagination-lg justify-content-center">
        {page > 1 
        ? <li className="page-item">
          <span 
            className="page-link page-number" 
            onClick={() => setPage(page - 1)}
          >
            Previous
          </span>
        </li>
        : ''}
          {pageNumbers.map((pageNo, index) => (
            <li key={index} className="page-item">
              <span 
                className="page-link page-number" 
                onClick={() => setPage(pageNo)}
              >
                {pageNo}
              </span>
            </li>
          ))}
          {page < pageNumbers.length 
          ? <li className="page-item">
            <span 
              className="page-link page-number" 
              onClick={() => setPage(page + 1)}
            >
              Next
            </span>
          </li>
          : ''}
        </ul>
      </nav>
    </div>
  );
}