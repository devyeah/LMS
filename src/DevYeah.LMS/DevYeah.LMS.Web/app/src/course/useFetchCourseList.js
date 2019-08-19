import {useState,useReducer, useEffect} from 'react';
import * as api from '../common/api';

const FETCH_INIT = "FETCH_INIT";
const FETCH_SUCCESS = 'FETCH_SUCCESS';
const FETCH_FAILURE = 'FETCH_FAILURE';

const getPageNumbers = (currentNumber, pageCount) => {
  let numbers = [];
  if (currentNumber.length !== pageCount) {
    for (let i = 1; i <= pageCount; i++) {
      numbers[i - 1] = i;
    }
    return numbers;
  }
  numbers = currentNumber.slice();
  return numbers;
}

const reducer = (state, action) => {
  switch (action.type) {
    case FETCH_INIT:
      return {
        ...state,
        isLoading: true,
        isError: false
      }
    case FETCH_SUCCESS:
      return {
        ...state,
        isLoading: false,
        isError: false,
        data: action.payload.results,
        pageNumbers: getPageNumbers(state.pageNumbers, action.payload.pageCount)
      }
    case FETCH_FAILURE:
      return {
        isLoading: false,
        isError: true,
      }
    default:
      return new Error();
  }
}

export default function useFetchCourseList(initialData) {
  const [page, setPage] = useState(process.env.REACT_APP_FIRST_PAGE);
  const [pageSize, setPageSize] = useState(process.env.REACT_APP_DFAULT_PAGESIZE);
  const [category, setCategory] = useState(null);
  const [state, dispatch] = useReducer(reducer, {
    isLoading: false,
    isError: false,
    data: initialData,
    pageNumbers: []
  });

  useEffect(() => {
    setPage(process.env.REACT_APP_FIRST_PAGE);
    setPageSize(process.env.REACT_APP_DFAULT_PAGESIZE);
  }, [category]);

  useEffect(() => {
    const fetchData = async () => {
      dispatch({type: FETCH_INIT});
      try {
        const response = await api.fetchCourses(page, pageSize, category);  
        dispatch({type: FETCH_SUCCESS, payload: response.data});
      } catch (error) {
        dispatch({type: FETCH_FAILURE});
      }
    };

    fetchData();
  }, [page, pageSize, category]);

  return {state, page, pageSize, setPage, setPageSize, setCategory};
}