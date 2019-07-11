import React, { useState, useEffect } from 'react';
import { useDropzone } from 'react-dropzone';
import defaultAvatar from '../images/user_pic-225x225.png';
import './account.css';

export default function PhotoUpload() {
  const [images, setImages] = useState([]);
  const {getRootProps, getInputProps} = useDropzone({
    accept: 'image/*',
    onDrop: acceptedImages => {
      setImages(acceptedImages.map(image => Object.assign(image, {
        preview: URL.createObjectURL(image),
      })));
    }
  });

  useEffect(() => () => {
    images.forEach(image => URL.revokeObjectURL(image.preview));
  }, [images]);

  const thumbs = images.map(image => (
    <img 
      key={image.name}
      alt="avatar"
      title="avatar"
      className="avatar"
      src={image.preview}
    />
  ));

  return (
    <div className="card">
      <div className="card-header font-weight-bold">
        Profile Photo
      </div>
      <div className="card-body container">
        <div className="row">
          <div className="col-4">
            {(images.length > 0)
              ? thumbs
              : (
                <img 
                  alt="avatar"
                  title="avatar"
                  className="avatar"
                  src={defaultAvatar}
                />
              )
            }
            <div className="mt-2">
              <button className="btn btn-danger btn-block font-weight-bold">
                save
              </button>
            </div>
          </div>
          <div className="col-8">
            <p id="tipsForUploadAvatar">
              Clear frontal face photos are an important way for other people to learn about you. 
              It’s not much fun to host a landscape! Be sure to use a photo that clearly shows your face and 
              doesn’t include any personal or sensitive info you’d rather not have others see.
            </p>
            <div {...getRootProps({className: 'dropzone'})}>
              <input {...getInputProps()} />
              <p id="avatarUploadPlaceHolder">Drag 'n' drop your photo here, or click to select files</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}