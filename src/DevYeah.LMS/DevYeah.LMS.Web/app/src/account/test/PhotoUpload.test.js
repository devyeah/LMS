import React from 'react';
import 'jest-dom/extend-expect';
import { render, cleanup } from '@testing-library/react';
import PhotoUpload from '../PhotoUpload';

afterEach(cleanup);

const setup = () => {
  const result = render(
    <PhotoUpload />
  );
  return result;
};

it('render photoUpload without crashing', () => {
  const { container } = setup();
  const fileNode = container.querySelector('#userAvatarPic');
  expect(fileNode.type).toBe('file');
});