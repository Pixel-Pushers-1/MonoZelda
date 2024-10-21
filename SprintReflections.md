# Pixel Pushers

## Sprint 2 Reflection

Our team demonstrated strong organization and proactivity in meeting the requirements of Sprint 2. As seen on our roadmap, most tasks were completed in a timely manner and were evenly distributed throughout the sprint. The player feature posed the most persistent challenge, as it was the most complex and had dependencies on several other aspects of the sprint.

Communication within the team was effective and frequent. An early implementation of a generic sprite drawing and loading system streamlined and unified sprite handling for the rest of the team. Additionally, the general adoption of the Command Pattern enabled us to integrate new functionality into the project with minimal effort. As our understanding of the pattern evolved, we iterated on its implementation and continue to refine it.

The Keyboard Controller saw considerable activity during the sprint but resisted refactoring due to the constant modifications. As a result, we anticipate a redesign of the Keyboard Controller in Sprint 3, aiming to simplify its interaction with the Command Manager and the command construction process.

Our team held weekly meetings on Monday evenings to share progress and discuss design ideas. We began Sprint 2 by populating a design board with the necessary tasks, which members volunteered to tackle. Throughout the sprint, we communicated effectively, especially when encountering challenges. We made extensive use of pull requests to maintain the quality of the codebase, and GitHub Actions ensured that all pull requests compiled successfully, preventing non-runnable code from being merged.

Looking ahead to Sprint 3, we do not foresee any substantial changes to our process. We plan to start the next sprint in the same manner—by filling the task board with necessary items and dividing up the work amongst team members.

Code reviews occurred organically as pull requests came in, as such, code metrics and code analysis were done in hindsight. We have identified a Github action that will run .Net code quality and style analysis for all pull requests to guarantee we are staying on top of those metrics.

## Sprint 3 Reflection

The team worked well together in Sprint 3 to work towards the functionality goals that we had set out to achieve. The burndown chart in the `README.md` shows that tasks were completed efficiently and no one person contributed an alarming amount more than another. The most challenging part of this sprint proved to be the addition of collision commands between the different game objects.

Through discord and PR reviews, communication on the team was exceptional and we all had a good idea of what everyone else was working on. The implementation of Collidables and the Collision Controller made implementing the functionality needed for Sprint 3 much simpler. We also continued to iterate on previous designs such as the Keyboard Controller and Commands.

Our team was rather busy this sprint when it came to personal schedules. Because of this, we had to alter meeting times and change the days we met on. With that being said, all teammates were able to attend and participate in the weekly meetings. This helped foster a well connected environment for the team to thrive in.

Similar to Sprint 2, we believe that our team works very well together and nothing should really change in that regard. In a technical sense, we have discussed starting earlier on implementation so no one feels pressed to finish by deadlines for Sprint 4.

The code reviews were made through the built in PR function in GitHub. While we did not opt to do original code reviews, we increased the significance on using the PRs as a place to leave comments and talk amongst one another to make adjustements and refactor as needed. We were able to utilize the built in GitHub commands such as requesting changes or approve while reviewing PRs from others on the team.
