import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../../Components/Page';
import { addCountry } from './CountryCommands';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.min.css';

export function NewCountry() {
  const [country, setCountry] = useState(null);

  const handleInputChange = (event) => {
    setCountry({
      id: null,
      name: event.target.value,
    });
  };

  const notify = (info) => {
    toast.info(info);
  };

  const handleSubmit = async () => {
    let error = await addCountry({
      id: null,
      name: country?.name,
    }).then(
      () => null,
      (country) => country,
    );

    if (error === null) {
      notify('Country added');
    } else {
      toast.error(
        <div>
          Country not added!
          <br />
          {error.message}
        </div>,
      );
    }
  };

  return (
    <Page title="Add a new country">
      <Fragment>
        <ToastContainer />
        <div>
          <form onSubmit={handleSubmit}>
            <label>Country name:</label>
            <input
              type="text"
              id="countryName"
              name="name"
              title="Country name"
              onChange={handleInputChange}
              placeholder="country name"
            />
            <input type="submit" value="Save" />
          </form>
        </div>
      </Fragment>
    </Page>
  );
}
