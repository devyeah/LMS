import React, { Component } from 'react';
import defaultAvatar from '../images/user_pic-225x225.png';
import './account.css';

export default class PhotoUpload extends Component {
  constructor(props) {
    super(props);

    this.fileInputRef = React.createRef();
    this.openFileDialog = this.openFileDialog.bind(this);
  }

  openFileDialog() {
    this.fileInputRef.current.click();
  }

  render() {
    return (
      <div className="card">
        <div className="card-header">
          Profile Photo
        </div>
        <div className="card-body container">
          <div className="row">
            <div className="col-4">
              <img 
                alt="avatar"
                title="avatar"
                className="avatar rounded-circle"
                src={defaultAvatar}
              />
            </div>
            <div className="col-8">
              <p>
                Clear frontal face photos are an important way for hosts and guests to learn about each other. 
                It’s not much fun to host a landscape! Be sure to use a photo that clearly shows your face and 
                doesn’t include any personal or sensitive info you’d rather not have hosts or guests see.
              </p>
              <button 
                id="avatarUploadBtn"
                type="button"
                className="btn btn-outline-secondary font-weight-bold btn-block"
                onClick={this.openFileDialog}
              >
                Upload a file from your computer
              </button>
              <input 
                ref={this.fileInputRef}
                id="userAvatarPic" 
                type="file" 
                accept="image/*" 
                hidden 
              />
            </div>
          </div>
        </div>
      </div>
    );
  }
  
}