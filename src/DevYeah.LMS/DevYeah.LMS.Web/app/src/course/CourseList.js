import React, {useCallback, useEffect} from 'react';
import useFetchCourseList from './useFetchCourseList';
import DataLoading from '../common/DataLoading';
import DataNotFound from '../common/DataNotFound';
import FetchDataError from '../common/FetchDataError';
import CourseItem from './CourseItem';
import './course.css';

export default function CourseList({activeCat}) {
  const {state, page, pageSize, setPage, setPageSize, setCategory} = useFetchCourseList([]);
  useEffect(() => {
    setCategory(activeCat);
  }, [activeCat, setCategory]);

  return (
    <div>
      {state.isError 
        ? <FetchDataError />
        : (state.isLoading 
          ? <DataLoading />
          : (state.data.length === 0 
            ? <DataNotFound />
            : (<div>
              {state.data.map((course, index) => (
                <CourseItem 
                  key={index}
                  course={course}
                />
              ))}
            </div>)))
      }
      {!state.isError 
        && 
        (<nav id="pagination" aria-label="Page navigation">
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
            {state.pageNumbers.map((pageNo, index) => (
              <li key={index} className="page-item">
                <span 
                  className="page-link page-number" 
                  onClick={() => setPage(pageNo)}
                >
                  {pageNo}
                </span>
              </li>
            ))}
            {page < state.pageNumbers.length 
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
        </nav>)
      }
    </div>
  );
}