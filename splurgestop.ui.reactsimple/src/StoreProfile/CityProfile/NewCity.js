import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../../Components/Page';
import { addCity } from './CityCommands';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.min.css';

export function NewCity() {
  const [city, setCity] = useState(null);

  const handleInputChange = (event) => {
    setCity({
      id: null,
      name: event.target.value,
    });
  };

  const notify = (info) => {
    toast.info(info);
  };

  const handleSubmit = async () => {
    let error = await addCity({
      id: null,
      name: city?.name,
    }).then(
      () => null,
      (city) => city,
    );

    if (error === null) {
      notify('City added');
    } else {
      toast.error(
        <div>
          City not added!
          <br />
          {error.message}
        </div>,
      );
    }
  };

  return (
    <Page title="Add a new city">
      <Fragment>
        <ToastContainer />
        <div>
          <form onSubmit={handleSubmit}>
            <label>City name:</label>
            <input
              type="text"
              id="cityName"
              name="name"
              title="City name"
              onChange={handleInputChange}
              placeholder="city name"
            />
            <input type="submit" value="Save" />
          </form>
        </div>
      </Fragment>
    </Page>
  );
}
