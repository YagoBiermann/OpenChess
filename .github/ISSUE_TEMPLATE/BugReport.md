name: 🐛 Bug Report
description: File a bug report here
title: "[Bug] "
labels: ["bug"]
assignees: ["YagoBiermann"]
body:
  - type: markdown
    attributes:
      value: |
       Hi! thanks for reporting the bug! Please, describe what happened.

  - type: textarea
    id: bug-description
    attributes:
      label: Description of the bug
      description: Give us a brief description of what happened and what should have happened.
      placeholder: |
        Tell us what you see!
    validations:
      required: true

  - type: textarea
    id: steps-to-reproduce
    attributes:
      label: Steps To Reproduce
      description: Steps to reproduce the behavior.
      placeholder: |
        1. Go to '...'
        2. Click on '...'
        3. Scroll down to '...'
        4. See error
    validations:
      required: true

  - type: dropdown
    id: browsers
    attributes:
      label: What browsers are you seeing the problem on?
      multiple: true
      options:
        - Firefox
        - Chrome
        - Safari
        - Microsoft Edge
        - Brave
        - Opera
        - Internet explorer
        - Other

  - type: textarea
    id: additional-information
    attributes:
      label: Additional Information
      description: |
        Provide any additional information such as logs, screenshots, likes, scenarios in which the bug occurs so that it facilitates resolving the issue.
