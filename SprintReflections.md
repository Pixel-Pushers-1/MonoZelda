# Pixel Pushers

## Sprint 5 Reflection

This sprint, we spent about the first week fixing bugs that were introduced when integrating sprint 4 features and adding in small features that were left behind from Sprint 4. We used in class discussions to brainstorm what features we wanted to work on for Sprint 5, which we documented in [this wiki page](https://github.com/Pixel-Pushers-1/MonoZelda/wiki/Sprint-5-Ideas). While doing this, we divded up tasks between group members to decide who would be working on what. This way, every group member had a significant feature to work on, and members that wanted a bit more of a challenge for this sprint were able to take on more challenging/larger features. Then, most of the sprint was spent with members working on their own separate features before coming together to merge features and combine functionality for the last few days of the sprint.

We had great communication this sprint through in class discussions, regular conversations in our Discord server, as well as more formal code reviews through github PRs. There were many PRs this sprint which ranged from small bug fixes to large, project-wide refactorings. We found that a few extremely large PRs (1,000+ lines of code), especially from branches that were quite old, introduced many merge conflicts and were overall a headache to deal with. Most of this was dealt with the night before the sprint deadline where we met in a voice chat to get all outstanding changes merged and fix any bugs that were introduced as a result. 

During sprint 5 we recieved our code quality feedback from sprint 4 and found that we had done a good job refactoring previously problematic classes and keeping code quality in mind when adding additional features. There was a significant effort this sprint to refactor code that wasn't up to quality, such as the projectile system and then item system. These changes made the systems much simpler and easier to interact with. 

Overall, we were able to complete all of the features we set out to implement for this sprint, and the team did a great job of hitting deadlines so that everything was ready to merge on time. By the nature of the features, very large changes were needed to be made in the project, so a large amount of effort was put into the project by all team members for this last sprint. Now, the project is in a place that we are all happy with from baseline functionality from the original NES Zelda, as well as the extra features we added on top for sprint 5.

## Sprint 4 Reflection 

The teamwork was good for Sprint 4. We noticed at the start of the sprint that the amount of tasks were significantly more than previous sprints, so everyone was proactive in taking tasks. In regards to workload this sprint was the heaviest because tasks in this sprint were also a bit more complicated that previous sprints due to which there are still some minor bugs in the sprint. The most worked on task this Sprint was dividing the game into scenes and implementing the HUD. 

Communication was great for this sprint. All team members were active on discord and communicated any issues they were facing. Code reviews were performed using pull requests on github. The amount of pull requests we made this sprint were significantly more than previous due to the workload. The team is planning to update the wiki on github to include design discussions that happen on discord. These design discussions should include details about any issues with readability or maintainability of code. 

The team didn't meet outside of class a lot this sprint becuase most of the discussions were performed in the class meeting times. The general breakdown of the sprint was similar to before. At the start tasks were planned out and put on the project board. The first week was spent on refactoring any code that the team wanted to change from Sprint3. The next 2 weeks were spent on completing the tasks on the project board. 

We expect Sprint 5 to be less time consuming than Sprint 4 mainly because the bugs in the game right now are not complicated and should not take much time to fix. We will spend the first few days of Sprint 5 refactoring any code we need to, depending on the feedback we get and what the team decides collectively. Task planning, task dividing and code reviews will remain the same for Sprint 5. We will also need to spend some extra time on deciding what extra features we want to implement in Sprint 5.

Overall, we made solid progress this Sprint and are looking forward to work on Sprint 5.

## Sprint 3 Reflection

The team worked well together in Sprint 3 to work towards the functionality goals that we had set out to achieve. The burndown chart in the `README.md` shows that tasks were completed efficiently and no one person contributed an alarming amount more than another. The most challenging part of this sprint proved to be the addition of collision commands between the different game objects.

Through discord and PR reviews, communication on the team was exceptional and we all had a good idea of what everyone else was working on. The implementation of Collidables and the Collision Controller made implementing the functionality needed for Sprint 3 much simpler. We also continued to iterate on previous designs such as the Keyboard Controller and Commands.

Our team was rather busy this sprint when it came to personal schedules. Because of this, we had to alter meeting times and change the days we met on. With that being said, all teammates were able to attend and participate in the weekly meetings. This helped foster a well connected environment for the team to thrive in.

Similar to Sprint 2, we believe that our team works very well together and nothing should really change in that regard. In a technical sense, we have discussed starting earlier on implementation so no one feels pressed to finish by deadlines for Sprint 4.

The code reviews were made through the built in PR function in GitHub. While we did not opt to do original code reviews, we increased the significance on using the PRs as a place to leave comments and talk amongst one another to make adjustements and refactor as needed. We were able to utilize the built in GitHub commands such as requesting changes or approve while reviewing PRs from others on the tea

## Sprint 2 Reflection

Our team demonstrated strong organization and proactivity in meeting the requirements of Sprint 2. As seen on our roadmap, most tasks were completed in a timely manner and were evenly distributed throughout the sprint. The player feature posed the most persistent challenge, as it was the most complex and had dependencies on several other aspects of the sprint.

Communication within the team was effective and frequent. An early implementation of a generic sprite drawing and loading system streamlined and unified sprite handling for the rest of the team. Additionally, the general adoption of the Command Pattern enabled us to integrate new functionality into the project with minimal effort. As our understanding of the pattern evolved, we iterated on its implementation and continue to refine it.

The Keyboard Controller saw considerable activity during the sprint but resisted refactoring due to the constant modifications. As a result, we anticipate a redesign of the Keyboard Controller in Sprint 3, aiming to simplify its interaction with the Command Manager and the command construction process.

Our team held weekly meetings on Monday evenings to share progress and discuss design ideas. We began Sprint 2 by populating a design board with the necessary tasks, which members volunteered to tackle. Throughout the sprint, we communicated effectively, especially when encountering challenges. We made extensive use of pull requests to maintain the quality of the codebase, and GitHub Actions ensured that all pull requests compiled successfully, preventing non-runnable code from being merged.

Looking ahead to Sprint 3, we do not foresee any substantial changes to our process. We plan to start the next sprint in the same manner—by filling the task board with necessary items and dividing up the work amongst team members.

Code reviews occurred organically as pull requests came in, as such, code metrics and code analysis were done in hindsight. We have identified a Github action that will run .Net code quality and style analysis for all pull requests to guarantee we are staying on top of those metrics.
