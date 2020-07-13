import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../../Components/Page';
import { addBrand } from './BrandCommands';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.min.css';

export function NewBrand() {
  const [brand, setBrand] = useState(null);

  const handleInputChange = (event) => {
    setBrand({
      id: null,
      name: event.target.value,
    });
  };

  const notify = (info) => {
    toast.info(info);
  };

  const handleSubmit = async () => {
    let error = await addBrand({
      id: null,
      name: brand?.name,
    }).then(
      () => null,
      (brand) => brand,
    );

    if (error === null) {
      notify('Brand added');
    } else {
      toast.error(
        <div>
          Brand not added!
          <br />
          {error.message}
        </div>,
      );
    }
  };

  return (
    <Page title="Add a new brand">
      <Fragment>
        <ToastContainer />
        <div>
          <form onSubmit={handleSubmit}>
            <label>Brand name:</label>
            <input
              type="text"
              id="brandName"
              name="name"
              title="Brand name"
              onChange={handleInputChange}
              placeholder="brand name"
            />
            <input type="submit" value="Save" />
          </form>
        </div>
      </Fragment>
    </Page>
  );
}
