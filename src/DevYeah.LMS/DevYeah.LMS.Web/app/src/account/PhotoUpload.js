import React, { useState, useEffect } from 'react';
import { useDropzone } from 'react-dropzone';
import * as api from '../common/api';
import Alert from './Alert';
import defaultAvatar from '../images/user_pic-225x225.png';
import './account.css';

export default function PhotoUpload() {
  const [images, setImages] = useState([]);
  const [avatarUrl, setAvatarUrl] = useState("");
  const [uploadErrorMsg, setUploadErrorMsg] = useState("");
  const [isDisableSaveBtn, setIsDisableSaveBtn] = useState(true);
  const {getRootProps, getInputProps} = useDropzone({
    accept: 'image/*',
    onDrop: acceptedImages => {
      setIsDisableSaveBtn(false);
      setImages(acceptedImages.map(image => Object.assign(image, {
        preview: URL.createObjectURL(image),
      })));
    }
  });

  useEffect(() => {
    api.fetchAvatar()
      .then(response => {
        setAvatarUrl(response.data);
      })
      .catch(error => {});
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

  function handleUploadImage() {
    if (images.length === 0) return;
    setIsDisableSaveBtn(true);
    const formData = new FormData();
    formData.append("image", images[0]);
    api.uploadImage(formData)
      .then(response => {
        setAvatarUrl(response.data);
        setImages([]);
      })
      .catch(error => {
        setUploadErrorMsg(error.request.responseText)
        setIsDisableSaveBtn(false);
      });
  }

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
                  src={avatarUrl ? avatarUrl : defaultAvatar}
                />
              )
            }
            {uploadErrorMsg 
              && (
                <Alert
                  message={uploadErrorMsg}
                  type="error"  
                />
              )
            }
            <div className="mt-2">
              <button 
                className="btn btn-danger btn-block font-weight-bold"
                onClick={handleUploadImage}
                disabled={isDisableSaveBtn}
              >
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
              <p id="avatarUploadPlaceHolder">Drag 'n' drop your photo here, or click to select photo</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}